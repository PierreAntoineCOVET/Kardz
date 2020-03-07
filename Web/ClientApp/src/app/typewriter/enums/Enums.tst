${
    Template(Settings settings)
    {
        settings.IncludeProject("Domain");
        settings.OutputFilenameFactory = file => $"..\\..\\Web\\ClientApp\\src\\app\\typewriter\\enums\\{file.Enums[0].Name}.enum.ts";
    }
}/*************************/
/* Auto generated script */
/*************************/
$Enums(*Enum)[
export enum $Name {
    $Values[
    $Name = $Value][,]
}
]
