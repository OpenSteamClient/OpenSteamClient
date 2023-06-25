#pragma once
#include <opensteamworks/steamutl/utlvector.h>
#include <vector>

namespace CUtl
{
    template <typename T>
    // Create a std::vector from a CUtlVector. Deletes the CUtlVector afterwards.
    std::vector<T> CUtlVectorToStdVector(CUtlVector<T> *vec)
    {
        std::vector<T> stdVec;
        for (size_t i = 0; i < vec->Count(); i++)
        {
            stdVec.push_back(vec->Element(i));
        }

        delete vec;
        return stdVec;
    }

    // Create a std::vector<std::string> from a CUtlVector<CUtlString>. Deletes the CUtlVector afterwards.
    std::vector<std::string> CUtlStrVectorToStdVector(CUtlVector<CUtlString> *vec)
    {
        std::vector<std::string> stdVec;
        for (size_t i = 0; i < vec->Count(); i++)
        {
            stdVec.push_back(vec->Element(i).str);
        }

        delete vec;
        return stdVec;
    }

    // Create a std::string from a CUtlString. Doesn't delete the CUtlString.
    std::string CUtlStringToStdString(CUtlString *str)
    {
        return std::string(str->str);
    }
} // namespace CUtl
