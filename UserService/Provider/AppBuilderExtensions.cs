using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Provider
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, ServiceEntity serviceEntity)
        {
          
            // 注册consul地址
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceEntity.ConsulIP}:{serviceEntity.ConsulPort}"));

            //服务信息
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//心跳检查
                HTTP = $"http://{serviceEntity.IP}:{serviceEntity.Port}/api/health",//健康检查地址,
                Timeout = TimeSpan.FromSeconds(5)
            };
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = serviceEntity.ServiceName,
                Address = serviceEntity.IP,
                Port = serviceEntity.Port,
                Tags = new[] { $"urlprefix-/{ serviceEntity.ServiceName}" }//添加urlprefix-/servicename格式的tag标签 方便Fabio识别
            };
            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动的注册,内部实现其实就是使用consulapi进行注册(httpclien发起)
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });
            return app;
        }
    }

    public class ServiceEntity
    {
        public string IP { get; set; }

        public int Port { get; set; }

        public string ServiceName { get; set; }

        public string ConsulIP { get; set; }

        public int ConsulPort { get; set; }
    }
}
