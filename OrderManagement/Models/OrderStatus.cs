using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagement.Models
{
    public enum OrderStatus
    {
        // 等待确认
        WaitForConfirm = 1,
       
        //确认假单
        ConfirmAsBad = 2,
       
        //等待发货
        WaitForSend = 3,
       
        //已经发货
        Sent = 4,
       
        // 已经签收
        Received = 5,
       
        // 已经退货
        Returned = 6
    }
}