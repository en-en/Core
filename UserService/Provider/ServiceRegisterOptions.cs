public class ServiceRegisterOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServiceName { get; set; }
    /// <summary>
    /// 服务IP或者域名
    /// </summary>
    public string ServiceHost { get; set; }
    /// <summary>
    /// 服务端口号
    /// </summary>
    public int ServicePort { get; set; }
    /// <summary>
    /// consul注册地址
    /// </summary>
    public RegisterOptions Register { get; set; }
}

public class RegisterOptions
{
    public string HttpEndpoint { get; set; }
}