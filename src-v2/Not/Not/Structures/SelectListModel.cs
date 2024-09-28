using Not.Extensions;

namespace Not.Structures;

public class SelectListModel
{
    public static IEnumerable<SelectListModel> FromEnum<T>()
         where T : struct
    {
        var enumValues = Enum.GetValues(typeof(T)).Cast<Enum>();
        var selectItems = enumValues.Select(s => new SelectListModel(s.ToString(), s.GetDescription()));
        return selectItems;
    }
    public SelectListModel(string value, string description)
    {
        Value = value;
        Description = description;
    }

    public string Value { get; set; }
    public string Description { get; set; }
}



