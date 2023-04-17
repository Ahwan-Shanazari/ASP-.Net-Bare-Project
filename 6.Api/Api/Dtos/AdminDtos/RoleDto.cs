namespace Api.Dtos.AdminDtos;

public class RoleDto
{
    public long RoleId { get; set; }
    public string Name { get; set; }
    
    public List<string> Permissions { get; set; }
    public RoleDto(long roleId,string name)
    {
        RoleId = roleId;
        Name = name;
    }
}