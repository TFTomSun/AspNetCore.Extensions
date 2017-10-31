namespace TomSun.AspNetCore.Extensions.TagHelpers
{
    public class TagId
    {
        public string Value { get; set; }
        public static implicit operator string(TagId id)
        {
            return id.Value;
        }

        public static implicit operator TagId(string value)
        {
            return new TagId {Value = value};
        }
    }
}