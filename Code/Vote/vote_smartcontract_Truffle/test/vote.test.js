var vote = artifacts.require('Vote');

contract("Vote test", async accounts=>{
   it("should have 0 votes after deployment", async()=>{
       let instance = await vote.deployed();
       let candidate1Votes = await instance.candidate1.call();
       let candidate2Votes = await instance.candidate2.call();
       assert.equal(candidate1Votes.valueOf(),0);
       assert.equal(candidate2Votes.valueOf(),0);
   });

   it("should vote for the candidates 1 with the correct values", async()=>{
        let instance = await vote.deployed();
        await instance.castVote(1);
        let candidate1Votes = await instance.candidate1.call();
        let candidate2Votes = await instance.candidate2.call();
        assert.equal(candidate1Votes.valueOf(),1);
        assert.equal(candidate2Votes.valueOf(),0);
    });

    
   it("should vote for the candidates 2 with the correct values", async()=>{
      let instance = await vote.deployed();
      await instance.castVote(2);
      let candidate1Votes = await instance.candidate1.call();
      let candidate2Votes = await instance.candidate2.call();
      assert.equal(candidate1Votes.valueOf(),1);
      assert.equal(candidate2Votes.valueOf(),1);
  });
})
