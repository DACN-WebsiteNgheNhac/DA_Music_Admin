using CloudinaryDotNet.Actions;
using DA_Music_Admin;
using Microsoft.EntityFrameworkCore;
using Services.Const;
using Services.IServices;
using System;
using System.Linq.Expressions;
using UploadService.Cloudinary.Services;
using UploadService.IServices;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly MusicContext _context;
        private readonly IPhotoUploadService _photoUploadService;

        public UserService(MusicContext context,
            IPhotoUploadService photoUploadService)
        {
            _context = context;
            _photoUploadService = photoUploadService;
        }
        public async Task<User> CreateObject(User data)
        {
            if (data == null)
                return null;

            var lastestIndex = await GetLastestIndex();
            var id = "U" + lastestIndex.ToString("000");

            data.Id = id;

            string image = null;
            string audio = null;

            if (!string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }

            if (string.IsNullOrEmpty(data.Password))
                data.Password = "123456";

            data.Image = image;
            data.CreatedAt = DateTimeOffset.Now;
            await _context.Set<User>().AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<User> EditUser(User data, bool newImage)
        {
            if (data == null)
                return null;

            string image = data.Image;

            if (newImage && !string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }


            data.Image = image;
            data.UpdatedAt = DateTimeOffset.Now;

            _context.Set<User>().Update(data);
            await _context.SaveChangesAsync();
            _context.Entry(data).State = EntityState.Detached;

            return data;
        }

        public Task<List<User>> GetListAlls()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalPages(string query, string gender, string roleId
            , DateTime fromDate, DateTime toDate, int pageSize = -1)
        {
            var totalPage = (await SearchUsers(query, gender, roleId, fromDate, toDate)).Count;
            return (int)Math.Ceiling(totalPage / (pageSize * 1.0));
        }

        public async Task<User> Login(string username, string password)
        {
            var _context = new MusicContext();
            return await _context.Set<User>().AsNoTracking()
                .Where(t => t.Username == username 
                && t.Password == password
                && t.RoleId == "0")
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> SearchUsers(string query, string gender, string roleId
            , DateTime fromDate, DateTime toDate, int pageNumber = -1, int pageSize = -1)
        {
            var _context = new MusicContext();
            query = string.IsNullOrEmpty(query) ? "" : query;
            gender = (string.IsNullOrEmpty(gender) || gender == "Chọn giới tính") ? "" : gender;
            roleId = (string.IsNullOrEmpty(roleId) || roleId == "Chọn quyền") ? "" : roleId;
            var FromDate = StartDate(fromDate);
            var ToDate = EndDate(toDate);
           
            Expression<Func<User, bool>> predicate =
                 t =>
                 (string.IsNullOrEmpty(t.Name) ? true : t.Name.Contains(query)
                 || t.Username.Contains(query))
                 && (string.IsNullOrEmpty(roleId) ? true : t.RoleId.Contains(roleId)
                 && t.DeletedAt == null);


            if (pageNumber > -1 && pageSize > -1)
                return await _context.Set<User>().AsNoTracking()
                    .OrderByDescending(t => t.CreatedAt)
                    .Where(predicate)
                    .Where(t => string.IsNullOrEmpty(t.Gender) ? true : t.Gender.Contains(gender))
                    .Where(t => t.CreatedAt >= FromDate)
                    .Where(t => t.CreatedAt <= ToDate)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .Include(t => t.Role)
                    .ToListAsync();
            else
            {
                return await _context.Set<User>().AsNoTracking()
                     .OrderByDescending(t => t.CreatedAt)
                     .Where(predicate)
                     .Include(t => t.Role)
                     .ToListAsync();
            }
        }

        DateTimeOffset StartDate(DateTime? date)
        {
            if(date != null)
            {
                if (date.Value.Year == 1)
                    date = date.Value.AddYears(1);
            }
            return date == null
                ? new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0), TimeSpan.FromHours(7))
                : new DateTimeOffset(date.Value.Year, date.Value.Month, date.Value.Day, 1, 0, 0, TimeSpan.FromHours(7));
        }

        DateTimeOffset EndDate(DateTime? date)
        {
            date = date == null ? DateTime.Now : date;
            return new DateTimeOffset(new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59));
        }

        public async Task<int> GetLastestIndex()
        {
            var lastObject = await _context.Set<User>().AsNoTracking()
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            if (lastObject == null)
                return 1;
            else
            {
                var prefix = "U";
                var split = lastObject.Id.Split(prefix);
                var id = 0;
                if (split.Count() <= 0)
                {
                    id = 1;
                }
                else
                {
                    var temp = int.Parse(split[1]);
                    id = ++temp;
                }
                return id;
            }

        }

        public async Task<string> UploadImage(string fileName, string publicId)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                var resultUpload = await _photoUploadService.AddPhotoAsync(fileName, fileStream
                    , new PhotoCloudinary
                    {
                        DestinationFolder = UploadConst.UserFolder.Image,
                        PublicId = publicId
                    });

                return ((ImageUploadResult)resultUpload).SecureUrl.ToString();
            }
        }


    }
}
