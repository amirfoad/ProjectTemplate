namespace Application.Dtos.Services
{/// <summary>
/// specify the Path for Get/Set Files
/// </summary>
    public class FolderSettings
    {
        /// <summary>
        /// Application root folder name
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// News folder name
        /// </summary>

        public string NewsFolder { get; set; }

        public string CourseFolder { get; set; }
        public string CourseResourceFolder { get; set; }
    }
}