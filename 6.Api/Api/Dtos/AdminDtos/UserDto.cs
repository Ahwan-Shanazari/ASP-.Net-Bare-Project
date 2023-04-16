namespace Api.Dtos.AdminDtos;

//ToDo: is this place okay for these type of DTOs or should I move them to another layer?
public class UserDto
{
    public long Id { get; set; }
    public List<string> CustomPermissions { get; set; }
    public List<string> Roles { get; set; }
}