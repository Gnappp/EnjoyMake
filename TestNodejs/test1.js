var express = require('express');
var http = require('http');
var mysql=require('mysql');
var port = 3000;
var app = express();
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
let connect=mysql.createConnection({
        host:'localhost',
        port:3306,
        user:'root',
        password:'1q2w3e4r',
        database:'test_nodejs',
    });

function SetRank(values)
{
    connect.query('insert into RANKING(name,time) values (?) ',[values],
    (error)=>
    {
        if(error)
            console.log('error: '+error);
    }
    );
}
function GetRank(res)
{
    connect.query('select * from RANKING order by time,name asc;',(error,rows,field)=>
    {
        if(error)
        {
            console.log("에러 발생: "+error);
            return;
        }
        res.json(rows);
    });
    
}


var server=http.createServer(app);
app.post('/SetRank',function (req,res){
    var values=[JSON.parse(JSON.stringify(req.body.name)),JSON.parse(JSON.stringify(req.body.time))];
    console.log(values[1]);
    SetRank(values);
    console.log("Come");
    res.send('완료');
});

app.post('/GetRank',function(req,res){
    GetRank(res);
})


app.get('/',function (req,res){
    res.send('hellow');
});

app.get('/Hellow',function(req,res){
    res.send('World');
});
server.listen(port,function(){
     console.log('Server running at http://127.0.0.1:3000/');
})

// var server = http.createServer((request,respone)=>{
//     respone.writeHead(200);
//     respone.end("Hellow World");
// }).listen(port);
// console.log('Server running at http://127.0.0.1:3000/');