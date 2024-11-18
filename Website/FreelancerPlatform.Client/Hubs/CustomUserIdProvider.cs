using Microsoft.AspNetCore.SignalR;
using FreelancerPlatform.Application.Extendsions;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        // Trả về tên người dùng hoặc bất kỳ thuộc tính nào mà bạn muốn dùng làm UserId
        //return connection.User?.Identity?.Name;
        return connection.User.GetUserId().ToString();
    }
}
