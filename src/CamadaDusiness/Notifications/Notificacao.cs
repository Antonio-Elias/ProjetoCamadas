using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CamadaBusiness.Notifications;

public class Notificacao
{
    public string Mensagem {  get; }

    public Notificacao(string mensagem)
    {
        Mensagem = mensagem;
    }
}
