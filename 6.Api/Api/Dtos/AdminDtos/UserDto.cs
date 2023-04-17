namespace Api.Dtos.AdminDtos;

//ToDo: is this place okay for these type of DTOs or should I move them to another layer?
public class UserDto
{
    //ToDo: also add the UserName Property to this dto 
    public long UserId { get; set; }
    public List<string> CustomPermissions { get; set; }
    public List<RoleDto> Roles { get; set; }
}