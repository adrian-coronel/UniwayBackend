namespace UniwayBackend.Config
{
    public static class Constants
    {
        public static readonly string[] VALID_CONTENT_TYPES = new string[] 
        { 
            "image/jpeg", 
            "image/png", 
            "image/jpg",
            "application/pdf"
        };
        public static readonly Dictionary<string, string> VALID_TYPES = new Dictionary<string, string>
        {
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".pdf", "application/pdf"}
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
            public const int RESPONDING = 6;

        }
        public static class TypeAttentionRequest
        {
            public const int URGENTE_ATTENTION = 1;
            public const int SCHEDULE_ATTENTION = 2;
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
            public const short PENDING = 1;
            public const short IN_PROCESS = 2;
            public const short CLOSED = 3;
            public const short CANCELED = 4;
            public const short CLOSURE_REQUEST = 5;
            public const short RESPONDING = 6;
            public const short SCHEDULED_ON_HOLD = 7;

            public static string GetName(short stateRequestId)
            {
                switch (stateRequestId)
                {
                    case PENDING:
                        return "Pendiente";
                    case IN_PROCESS:
                        return "En proceso";
                    case CANCELED:
                        return "Cancelado";
                    case CLOSED:
                        return "Culminado";
                    case CLOSURE_REQUEST:
                        return "Solicitud de culminado";
                    case RESPONDING:
                        return "Respondido";
                    default:
                        return "No se encontro el estado de la solicitud";
                }
            }
        }

        public static class TypesMethodsConnection
        {
            public const string RECEIVE_NOTIFICATION_REQUESTS = "ReceiveNotificationRequests";
            public const string RECEIVE_NOTIFICATION_TECH_RESP = "ReceiveNotificationTechnicalResponse";
            public const string RECEIVE_NOTIFICATION_CHANGE_STATE_REQUEST = "NotificationChangeStateRequest";
        }

        public static class TypesConnectionSignalR
        {
            public const string SOLICITUDE = "Solicitude";
            public const string RESPONSE = "Response";
            public const string CLOSE_SOLICITUDE = "Solicitud Cierre";

        }

        public static class EntityTypes
        {
            public const string MECHANICAL = "Mecánico";
            public const string WORKSHOP = "Taller";
            public const string CLIENT = "Cliente";
        }
    }
}
