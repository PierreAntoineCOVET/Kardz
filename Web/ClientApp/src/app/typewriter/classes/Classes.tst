${
    Template(Settings settings)
    {
        settings.IncludeProject("DTOs");
    }

    string MapType(Property p)
    {
        if(TypeMapper.Any(mt => mt.Key == p.Type.FullName)) 
        {
            var mappedType = TypeMapper[p.Type.FullName];
            return mappedType.tsTypeName;
        }

        return "any";
    }

    string Import(Class c)
    {
        var types = c.Properties
            .Select(p => p.Type).Distinct()
            .Where(t => TypeMapper.Any(tm => tm.Key == t.FullName));

        return string.Join(
            Environment.NewLine, 
            types.Select(t => TypeMapper[t.FullName].tsTypeImport));
    }

    static IDictionary<string, (string tsTypeName, string tsTypeImport)> TypeMapper = new Dictionary<string, (string tsTypeName, string tsTypeImport)>
    {
        { "System.Guid", ("uuidv4", "import { v4 as uuidv4 } from 'uuid';") }
    };
}/*************************/
/* Auto generated script */
/*************************/
$Classes(*Dto)[
$Import

export class $Name {
    $Properties[
    public $name: $MapType;]
}
]
