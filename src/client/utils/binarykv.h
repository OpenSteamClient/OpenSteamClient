#include <nlohmann/json.hpp>
#include <vector>

// Small binarykv parser custom written for opensteamclient
class BinaryKV
{
private:
    /* data */
public:
    nlohmann::json outputJSON;
    // Creates a BinaryKV object from the 
    BinaryKV(std::vector<uint8_t> data);
    ~BinaryKV();
};