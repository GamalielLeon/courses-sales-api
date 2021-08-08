namespace Domain.Constants
{
    public static class GlobalConstants
    {
        public const string GENERIC_ENDPOINT = "api/[controller]";
        public const string COURSES_WITH_INSTRUCTORS_AND_PRICE = "CoursesWithInstructorsAndPrice";
        public const string COURSES_WITH_ALL_DETAILS = "CoursesWithAllDetails";
        public const string AUTHORIZACION = "Authorization";
        public const string BEARER = "Bearer ";
        public const string YYYY_MM_DD = "yyyy-MM-dd";
        public const string APPLICATION_JSON = "application/json";
        public const int MAX_PAGE_SIZE = 100;
        public const int DEFAULT_PAGE_SIZE_10 = 10;
        public const string DEFAULT_SORT_BY_ID = "Id";

        // Roles 3 and 4 cannot edit its role. Only the system itself (or any admin) can change their role.
        // Admins cannot be super-admins but are capable of owning other roles.
        public const string SUPER_ADMIN_ROLE = "sadm";      // 1 - All permissions.
        public const string ADMIN_ROLE = "adm";             // 2 - Cannot edit other admins adn very specific access restrictions.
        public const string INSTRUCTOR_ROLE = "teach";      // 3 - Almost the same as user, but can create and manage courses*.
        public const string USER_ROLE = "usr";              // 4 - Least permissions*.

        public const string ROLES_ALLOWED_FOR_COURSES_CONTROLLER = USER_ROLE;
        public const string ROLES_ALLOWED_FOR_COMMENTS_CONTROLLER = USER_ROLE;
        public const string ROLES_ALLOWED_FOR_PRICES_CONTROLLER = SUPER_ADMIN_ROLE;
        public const string ROLES_ALLOWED_FOR_ROLES_CONTROLLER = SUPER_ADMIN_ROLE;
        public const string ROLES_ALLOWED_FOR_USER_ROLES_CONTROLLER = SUPER_ADMIN_ROLE + "," + ADMIN_ROLE;
        public const string ROLES_ALLOWED_FOR_USERS_CONTROLLER = SUPER_ADMIN_ROLE + "," + ADMIN_ROLE;
        public const string ROLES_ALLOWED_FOR_INSTRUCTORS_CONTROLLER = SUPER_ADMIN_ROLE + "," + ADMIN_ROLE;

        public const string ROLES_ALLOWED_FOR_CREATE_UPDATE_COURSES = SUPER_ADMIN_ROLE + "," + ADMIN_ROLE + "," + INSTRUCTOR_ROLE;
    }
}
