﻿namespace Application.Dtos.Authentications.Request
{
    public record ActionDescriptionDto
    {
        public string Key => $"{AreaName}:{ControllerName}:{ActionName}";

        public string AreaName { get; set; }

        public string ControllerName { get; set; }
        public string ControllerDisplayName { get; set; }

        public string ActionName { get; set; }

        public string ActionDisplayName { get; set; }
    }
}