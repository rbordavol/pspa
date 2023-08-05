public class KeyConfiguration{}
public class AccessKeyConfiguration{}
public class AccessSecretConfirguration { }
public class BucketConfiguration{}

public class AWSOptions 
{
    public const string AWS = "AWS";

    public string Profile { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string AccessSecret { get; set; } = string.Empty;
}

