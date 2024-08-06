using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace my_mini_project.ViewModel;

public class UserViewModel
{  
    [BsonId]
    public ObjectId Id { get; set; } // MongoDB ObjectId
    public string username { get; set; } = null!;
    public string password { get; set; } = null!;
    public string firstname { get; set; } = null!;
    public string lastname { get; set; } = null!;
    public string email { get; set; } = null!;
    public string usecode { get; set;} = null;
    public string selfcode { get; set;} = null;
    public int descending { get; set;} = -1;
    public int gain { get; set;} = 0;

}
public class UserSignUp
{  
    public required string username { get; set; } 
    public required string password { get; set; } 
    public required string firstname { get; set; } 
    public required string lastname { get; set; } 
    public required string email { get; set; } 
    public string code { get; set;} = null;

}
