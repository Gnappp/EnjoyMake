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

function SetRank(req,res)
{
    console.log(req.body);
    var values=[JSON.parse(JSON.stringify(req.body.name)),JSON.parse(JSON.stringify(req.body.time))];
    connect.query('insert into RANKING(name,time) values (?);',[values],
    (error)=>
    {
        if(error)
        {
            console.log('error: '+error);
            res.send(error);
        }
        else
        { 
            res.json("success");
        }
    }
    );
    connect.query('DELETE FROM RANKING WHERE time IN (SELECT * FROM (SELECT MAX(time) FROM RANKING) AS t) AND (SELECT * FROM (SELECT COUNT(name) FROM RANKING) AS n) >15;',
    (error)=>
    {
        if(error)
        {
            console.log('error: '+error);
        }
    }
    );
}

function GetRank(req,res)
{
    connect.query('select * from RANKING order by time*1,name asc;',(error,rows,field)=>
    {
        if(error)
        {
            console.log("에러 발생: "+error);
            //res.send('error');
        }
        else 
        {
             res.json(rows);
        }
    });
    
}


var server=http.createServer(app);
app.post('/SetRank',function (req,res){
    SetRank(req,res);
    console.log("Come");
});

app.post('/GetRank',function(req,res){
    GetRank(req,res);
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
