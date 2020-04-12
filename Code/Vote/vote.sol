pragma solidity ^0.5.0;

 contract Vote {

     uint public candidate1;
     uint public candidate2;
     mapping (address => bool) public voted;

     function castVote(uint candidate) public  {
         require(!voted[msg.sender] && (candidate == 1 || candidate == 2));
         if(candidate == 1){
             candidate1++;
         }else{
             candidate2++;            
         }
         voted[msg.sender] = true;
     }
 }