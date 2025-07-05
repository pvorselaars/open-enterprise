using Google.Protobuf;

namespace OpenEnterprise.Ontology;

public partial class Role
{
    public Role(string name) => Name = name;

    public bool Is(Role role)
    {
        if (Name == role.Name)
            return true;

        if (Roles.Where(r => r.Name == role.Name).Any())
            return true;

        return false;
    }
}