# ShaniDEV
This is a solution to WSC Sports PartB test.
It uses Consumer Producer Design Pattern where the connections between the modules made by a queue (RabbitMQ) and DB (Relational - SQL Server)
Things I have'nt finished:
1. Add a Dependency Injection mechanisem (using Unity, or a straight-forward approach) where a third party service or db is used.
2. The exception handling is not completed
3. Logging 
4. I used console applications for easy debugging. Projects like "ProducerConsole" Should be in a Windows service.


Common Errors:
1. RabbitMQ dies. Solution: Set the config param to durable
2. One of the workflows tasks throws exception. 
   Solution: Use retries. If failed, write to log and move to an errors queue who will be handles aside.
3. The consumer dies during workflow excecution.
    Solution: The queue will remember the message on the next run. Using the statuses it can find the latest process that completed and         start from there.  
    
Shani Cohen
24.6.17
