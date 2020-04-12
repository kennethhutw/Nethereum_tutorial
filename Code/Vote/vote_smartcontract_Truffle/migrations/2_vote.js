var vote = artifacts.require("Vote");

module.exports = function (deployer) {
    // deployment steps
    deployer.deploy(vote);
};