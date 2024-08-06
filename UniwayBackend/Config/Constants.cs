namespace UniwayBackend.Config
{
    public static class Constants
    {
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

    }
}
