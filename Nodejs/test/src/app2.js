var express = require('express');
var http = require('http');
const mysql=require('mysql)';

let connect=mysql.connect(
    {
        host:'localhost',
        port:3306,
        user:'root',
        password:'1q2w3e4r',
        database:'test_nodejs',
    }
)

function SetRank(){
connect.query('insert into RANKING(name,time) values(?,?);',"Gna","5.33",
(error)=>
{
    if(error)
        console.log('error');
}
);
}

var port = 3000;
var app = express();

var server = http.createServer(app)

app.get('/setrank',function(req,res)
{
    SetRank();
})