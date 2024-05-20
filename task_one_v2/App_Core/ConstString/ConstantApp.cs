namespace task_one_v2.App_Core.ConstString
{
    static public class ConstantApp
    {
        public const string appVersion = "1.0.0";
        public static DateTime startDate = new DateTime(2000, 10, 15); 
        public const string imgPath = "Images";
        public const string wwwImgPath = "~/Images/";

        public const string userSessionKey = "user-session-key";
        public const string userCredentialsSessionKey = "user-credentials-session-key";

        public const string chef = "chef";
        public const string approved = "approved";

        public const string chefLayout = "~/Views/Shared/_ChefLayout.cshtml";
        public const string adminLayout = "~/Views/Shared/_AdminLayout.cshtml";
        public const string homeLayout = "~/Views/Shared/_HomeLayout.cshtml";
        public const string authLayout = "~/Views/Shared/_AuthLayout.cshtml";

        public const int chefRole = 3;
        public const int userRole = 2;
        public const int adminRole = 1;



        public const int approvedChef = 3;
        public const int rejectedChef = 2;
        public const int pendingChef = 1;



        public const int approvedRecipe = 3;
        public const int rejectedRecipe = 2;
        public const int pendingRecipe = 1;


        public const int approvedTestimonial = 1;
        public const int pendingTestimonial = 2;
        public const int rejectedTestimonial = 3;

    }




}
