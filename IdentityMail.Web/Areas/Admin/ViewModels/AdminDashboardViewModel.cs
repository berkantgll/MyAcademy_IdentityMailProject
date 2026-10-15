
namespace IdentityMail.Web.Areas.Admin.ViewModels
{
    public class AdminDashboardViewModel
    {
        // KULLANICI İSTATİSTİKLERİ
        public int TotalUsers { get; set; }

        public int ActiveUsers { get; set; }


        // MESAJ İSTATİSTİKLERİ
        public int TotalMessages { get; set; }

        public int TodayMessages { get; set; }

        public int UnreadMessages { get; set; }

        public int TrashedMessages { get; set; }


        // EN ÇOK MESAJ GÖNDERENLER
        public List<TopSenderViewModel> TopSenders { get; set; }
            = new List<TopSenderViewModel>();


        // EN ÇOK KULLANILAN KATEGORİLER
        public List<TopCategoryViewModel> TopCategories { get; set; }
            = new List<TopCategoryViewModel>();
    }


    public class TopSenderViewModel
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int MessageCount { get; set; }
    }


    public class TopCategoryViewModel
    {
        public string CategoryName { get; set; } = string.Empty;

        public int UsageCount { get; set; }
    }
}
