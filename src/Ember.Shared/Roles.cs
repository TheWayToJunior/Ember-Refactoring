namespace Ember.Shared
{
    public static class Roles
    {
        public const string User = nameof(User);

        public const string Consumer = nameof(Consumer);

        public const string Editor = nameof(Editor);

        public const string Admin = nameof(Admin);
    }

    public static class Policies
    {
        public const string RequireEditorRole = "RequireEditorRole";

        public const string RequireConsumersRole = "RequireConsumersRole";
    }
}
