namespace UniwayBackend.Config
{
    public static class Constants
    {
        public static readonly string[] VALID_CONTENT_TYPES = new string[] { "image/jpeg", "image/png", "image/jpg" };
        public static readonly Dictionary<string, string> VALID_TYPES = new Dictionary<string, string>
        {
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" }
        };
        public static short MAX_FILES = 5;
        public static int MAX_MB = 10; // 10 MB en bytes

        public static class StateRequestConfig
        {
            public const int PENDING = 1;
            public const int IN_PROCESS = 2;
            public const int CANCELED = 4;
            public const int CLOSED = 3;
            public const int CLOSURE_REQUEST = 5;
        }
        public static class Roles
        {
 
            private static readonly Dictionary<short, string> roleNames = new Dictionary<short, string>
            {
                { CLIENT_ID, CLIENT },
                { TECHNICAL_ID, TECHNICAL },
                { EMPLOYEE_ID, EMPLOYEE }
            };   
            


            public const string CLIENT = "CLIENT";
            public const string TECHNICAL = "TECHNICAL";
            public const string EMPLOYEE = "EMPLOYEE";

            public const short CLIENT_ID = 1;
            public const short TECHNICAL_ID = 2;
            public const short EMPLOYEE_ID = 3;

            public static string GetRoleName(short roleId)
            {
                if (roleNames.TryGetValue(roleId, out string roleName))
                {
                    return roleName;
                }
                return null;
            }
        }

        public static class State
        {
            public const bool INACTIVE_BOOL = false;
            public const bool ACTIVE_BOOL = true;

            public const byte INACTIVE_BYTE = 0;
            public const byte ACTIVE_BYTE = 1;
        }

        public static class Availabilities
        {
            public const short AT_HOME_ID = 1;
            public const short IN_WORKSHOP_ID = 2;
            public const short BOTH_ID = 0;
        }

        public static class StateRequests
        {
            public const int PENDING = 1;
            public const int IN_PROCESS = 2;
            public const int CLOSED = 3;
            public const int CANCELED = 4;
            public const int CLOSURE_REQUEST = 5;
        }

        public static class TypesMethodsConnection
        {
            public const string RECEIVE_NOTIFICATION_REQUESTS = "ReceiveNotificationRequests";
        }

        public static class TypesConnectionSignalR
        {
            public const string SOLICITUDE = "Solicitude";
        }
    }
}
