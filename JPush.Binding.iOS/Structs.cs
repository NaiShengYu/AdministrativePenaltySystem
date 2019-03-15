using System;
using ObjCRuntime;
using Foundation;
namespace JPush.Binding.iOS
{
    [Native]
    public enum JPAuthorizationOptions : uint
    {
    None = 0,
    Badge = (1 << 0),
    Sound = (1 << 1),
    Alert = (1 << 2)
    }
}
