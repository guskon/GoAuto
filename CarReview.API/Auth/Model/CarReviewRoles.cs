namespace CarReview.API.Auth.Model
{
    public static class CarReviewRoles
    {
        public const string Admin = nameof(Admin);
        public const string ReviewUser = nameof(ReviewUser);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, ReviewUser };
    }
}
