namespace Services.Const
{
    public class UploadConst
    {
        static readonly string RootFolder = "Music";
        static readonly string FolderImage = "/Image";
        static readonly string FolderAudio = "/Audio";

        public static readonly string PrefixImage = "img_";
        public static readonly string PrefixAudio = "au_";

        public class SongFolder
        {
            static readonly string BaseFolder = RootFolder + "/Song";
            public static string Image = BaseFolder + FolderImage;
            public static string Audio = BaseFolder + FolderAudio;
        }

        public class ArtistFolder
        {
            static readonly string BaseFolder = RootFolder + "/Artist";
            public static string Image = BaseFolder + FolderImage;
        }

        public class AlbumFolder
        {
            static readonly string BaseFolder = RootFolder + "/Album";
            public static string Image = BaseFolder + FolderImage;
        }  
        
        public class UserFolder
        {
            static readonly string BaseFolder = RootFolder + "/User";
            public static string Image = BaseFolder + FolderImage;
        }

    }


}
