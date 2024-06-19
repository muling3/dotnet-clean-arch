using Microsoft.AspNetCore.Identity;

namespace clean_arch.Domain.Entities;

public class UserToken : IdentityUserToken<Guid> { }