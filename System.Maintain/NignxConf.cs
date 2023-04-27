using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Maintain
{
    public class NignxConf
    {
        public static string Template = @"
worker_processes  4;
events {
    worker_connections  1024;
}
http {
    include       mime.types;
    default_type  application/octet-stream;

    sendfile        on;

    keepalive_timeout  15000;

    gzip  on;
    gzip_http_version 1.0;
    gzip_disable 'MSIE[1-6].';
    gzip_types text/css text/javascript application/javascript image/jpeg image/png image/gif image/jpg image/bmp;
    gzip_buffers 4 8k;
    gzip_min_length 1k;
    gzip_comp_level 9;
    gzip_vary on;

     {0}

    server {
        listen       8080;
        server_name  www.jjwf.com;

         {1}

       error_page   500 502 503 504  /50x.html;
       location = /50x.html
        {
            root   html;
         }
      }
}";
    }
}
