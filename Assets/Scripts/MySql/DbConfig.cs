namespace Assets.Scripts.MySql
{
    public class DbConfig
    {
        public const string DbName = "unitydemo"; //数据库名称。
        private const string Port = "3306";//默认端口
        
        //本地配置
        private const string LocalHost = "127.0.0.1"; //IP地址
        private const string LocalUserId = "root"; //用户名
        private const string LocalPwd = "930705"; //密码

        //远程配置
        private const string RemoteHost = "192.168.1.185"; //IP地址 
        private const string RemoteUserId = "zlh"; //用户名
        private const string RemotePwd = "123456"; //密码

        public static string GetLocalConnectString()
        {
            return $"server={LocalHost};port={Port};User Id={LocalUserId};password={LocalPwd};";
        }

        public static string GetRemoteConnectString()
        {
            return $"server={RemoteHost};port={Port};User Id={RemoteUserId};password={RemotePwd};";
        }

        public static string GetLocalConnectDbString()
        {
            return $"server={LocalHost};port={Port};User Id={LocalUserId};password={LocalPwd};Database={DbName}";
        }

        public static string GetRemoteConnectDbString()
        {
            return $"server={RemoteHost};port={Port};User Id={RemoteUserId};password={RemotePwd};Database={DbName}";
        }
    }
}