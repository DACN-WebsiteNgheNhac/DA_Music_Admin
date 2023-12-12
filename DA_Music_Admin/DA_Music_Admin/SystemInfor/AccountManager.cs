namespace DA_Music_Admin.SystemInfor
{
    public class AccountManager
    {
        public User Account { get; private set; }
        
        public void SaveAccount(User account)
        {
            this.Account = account;
        }
    }
}
