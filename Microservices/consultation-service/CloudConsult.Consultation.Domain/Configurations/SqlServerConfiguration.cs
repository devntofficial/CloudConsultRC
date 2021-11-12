﻿namespace CloudConsult.Consultation.Domain.Configurations
{
    public class SqlServerConfiguration
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool MultipleActiveResultSets { get; set; }
    }
}