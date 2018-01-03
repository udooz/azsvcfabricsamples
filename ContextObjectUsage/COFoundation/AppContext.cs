namespace COFoundation
{
    using System;

    public class AppContext
    {
        [ThreadStatic]
        private static AppContext current;

        public string UserId { get; set; }
        public string DeviceId { get; set; }

        public static AppContext Current
        {            
            get
            {                
                return current;
            }
        }

        public static void SetContext(string uid, string did)
        {
            AppContext.current = new AppContext
            {
                UserId = uid,
                DeviceId = did
            };
        }
    }
}
