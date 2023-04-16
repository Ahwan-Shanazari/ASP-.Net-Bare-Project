namespace Api.Dtos.AdminDtos;

public class RoleDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public List<string> Permissions { get; set; }
    public RoleDto(long id,string name)
    {
        Id = id;
        Name = name;
    }
}