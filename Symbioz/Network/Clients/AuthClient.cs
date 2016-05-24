using Symbioz.Auth.Models;
using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Helper;
using Symbioz.Network.Servers;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Network.Clients
{
    public class AuthClient : DofusClient
    {
        public byte[] AesKey { get; set; }

        public const string DofusPublicKey = @"AQ1dTCoUJAAB42n1V21LbRhje9UlaG4M5BAKEYAIJCQHZkEOLStwYsINTgjuY0ExmMnglrUBBljw6AL5qxjed6Vu0T9A3aG960yuSTNu7XnemN30D+q/khkNpNOD9z/vv938rHSHhGKGhJkLXMVrtHUYIfd3/C0ZoydF0eXO1nD1qmJYrg/Zoes/zmnIud3h4KB3ek2xnNze/uLiYyy/kFhbmIGLObVkePZqz3MnpQlBglbmqYzQ9w7ayXKeK7XuPpqc7VTX1Q9Gm75hBSU3NMZM1mOW5uXlpHgppqqzbToN6BdpsmoZKebnc0Zy7Z6v7h/SAzekmdfeWcqeBPMczPJMVipqtsGzZZEfZ+9niaX4QHYbwYO200cKZY1KeLal2I9d0bM1XoScdSgXJZ1N4iaavmIa7x5yCb+1b9mG4xamVx6gOo559PuJfG/eb1Nr16S4rlDYC3wc96JF6rPDUt7IPZrML+fkHYRfcuJS7AHbHAvMroNXMn9EltBI5OTl5mYzCgBPwH0t89xIFj/3tDz89g4H/nHxGDQv9OPCVgBDoSHdog82jDJJQ5O/vYwgtoxgPSQdoS5rhNk3aStSajuGxVGj0PcN0+3eZt8p0wzJ4K8utDagzARhK1NqnDboLqitptu67srzsG6ZWsXTbnb8Y8Zo5dN+wWFhUcluuxxqyXDScmkotiznypTUli3mHtrMvMctvwA4rpgFcqliuR01zq9VkJbDf/t/NAE8qyy8a5opt6cbu2uV7mPauoQIvoA2VH1JqUAvG5MB+Rd/bgw0NvUO0Z6EnBaAETVgqm+gw7JRdxdq93EI+/zCnAB6eYaV2dsC0syMdMHXgHN5yiPfUeeNquFaV19APdO7BnJgzfj6oYnlwSuj3gIWBox8pci30sQN+EeUSX7ifeipw+f7lqOwzx2ImHwDA/gEbd41amsmcKxw0a5uahhYAUwy8YpO6LsxLE30X0qGaCOsBcypahvqeXYOXgerVAlOSU6pYqn3BWolwWfo4BTwYtwsYOi7sJMvboVA68pilMS2+Akd008vPK+urO9ulzVqluhFv0Ne2E28Ylu0IDuxNXdZnuDUPLikYdzslusOkzdJ2hWelQvXL4tbKGuEj1DjRMivrldLG1k5ta7NUfFbZeJLuGJafb6yulxJ71AUyJzvGYmWzqyOW14u1tX5+2Asdk+WWx4qOQ1viIWfB861yMhC43U1sA1K2EzUsT2zabnD7ROhb495Y03f3uhUeVzyghkkVk619HD2wuEBd9wzPZbminSN3GNKvBpdF4q8rSfUdB2JEIHzJ8pxWzIeGhvlpLs1NQ9wpWWIuHDQRUrDrLAV7LlAwfY6tvf/h9uDlV6J3EA9Gr0YG43EUR0NYTA92j0RGekYyI70jfSP9g48iPfFYAgsiiUSTqa50d09ewBEhEhNiCdKFyRVMBjEZwuQqJsOYjGAyigU8Rq5jMo5JFpMJTG5gMonJFCY3MbmFyTQmtzG5g8kMJnexEJsV8JyAJZKDxLxAcmNZPC7geQEvCOI9Ad8nn4DjU7KIiYzJZyAvCZmCEPlciDwWIsUoOvtgHCHR4G2NIxjHMAIFiyR6nH8CplSGJP+IHOePK+hVdFYoR3H7VxBELvwGAuHC7yAkuaDGQEqBVI6hthoHZSAwJ5S4HtcTbVUIBKGtioEgtlUSCKStJpV4NYlAS7XVFGReg8x61zfLqD2jdr3V03o3/+nhP5l35V7UB9+YOdzOwFflrd7Xfqv3c9dA4BIReq9fCXzv9cH2u/JQx3a1YxtuV0fEV6NPR9Gb0Rk1rYjVa1hJK0K4pKbQVLY6Fq1fr4/X8DJ6M6F2K+kpRG9kUggRBZTyJKpOQax+cwoN/HVyAg0/ONNwj9KlxACE2YflWxjiiZKcnp5CUHCaF6zeJkqifAcpPdUZDDBHAHlx8jjPUP1uvj6br8/l61K+nsvXI/D3Ao0Fzx4PJf18VMF39jEI/wAYbt6Y";

        public AuthClient(Socket socket):base(socket)
        {
            this.SSyncClient.OnClosed += SSyncClient_OnClosed;

            Send(new ProtocolRequired(ConstantsRepertory.DOFUS_PROTOCOL_VERSION, ConstantsRepertory.DOFUS_PROTOCOL_VERSION));
            Send(new RawDataMessage(Convert.FromBase64String(new string(DofusPublicKey.Skip(1).ToArray()))));    
        }

        void SSyncClient_OnClosed()
        {
            AuthServer.Instance.RemoveClient(this);
        }
    }
}
