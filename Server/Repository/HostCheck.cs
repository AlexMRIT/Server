using System;
using System.Net;
using System.Linq;
using Server.Utilite;
using System.Threading;
using System.ServiceProcess;
using System.Net.NetworkInformation;

namespace Server.Repository
{
    public static class HostCheck
    {
        public static bool IsPingSuccessful(string host, int timeoutMs)
        {
            try
            {
                PingReply pingReply = new Ping().Send(host, timeoutMs, new byte[1]);
                return (pingReply != null) && (pingReply.Status == IPStatus.Success);
            }
            catch (Exception exception)
            {
                ExceptionHandler.Execute(exception, nameof(IsPingSuccessful));
                return false;
            }
        }

        public static bool IsLocalIpAddress(string host)
        {
            try
            {
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress hostIp in hostIPs)
                {
                    if (IPAddress.IsLoopback(hostIp))
                        return true;

                    if (localIPs.Contains(hostIp))
                        return true;
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.Execute(exception, nameof(IsLocalIpAddress));
            }

            return false;
        }

        public static bool ServiceExists(string serviceName)
        {
            try
            {
                return ServiceController.GetServices().Any(service => service.ServiceName.StartsWithIgnoreCase(serviceName));
            }
            catch (Exception exception)
            {
                ExceptionHandler.Execute(exception, nameof(ServiceExists));
            }

            return false;
        }

        public static bool IsServiceRunning(string serviceName)
        {
            try
            {
                return ServiceController.GetServices().Any(service => service.ServiceName.StartsWithIgnoreCase(serviceName) && (service.Status == ServiceControllerStatus.Running));
            }
            catch (Exception exception)
            {
                ExceptionHandler.Execute(exception, nameof(IsServiceRunning));
            }

            return false;
        }
        
        public static void StartService(string serviceName, int timeoutMs)
        {
            ServiceController service = ServiceController.GetServices().FirstOrDefault(filter => filter.ServiceName.StartsWithIgnoreCase(serviceName));

            if (service == null)
                return;

            switch (service.Status)
            {
                case ServiceControllerStatus.Running:
                    break;
                case ServiceControllerStatus.Stopped:
                    try
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMs);
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.Execute(exception, nameof(StartService));
                    }
                    break;
                default:
                    Thread.Sleep(timeoutMs);
                    break;
            }
        }
    }
}