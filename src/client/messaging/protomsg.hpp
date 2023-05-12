#include <steammessages_base.pb.h>
#include <enums_clientserver.pb.h>
#include <QDataStream>
#include <QBuffer>
#include <QByteArray>
#include <stdio.h>
#include <iomanip>
#include <thread>
#include "../globals.h"
#include "../ext/steamclient.h"
#include "../gui/application.h"
#include "../utils/jslikebuffer.h"
#include <opensteamworks/IClientUser.h>
#include <opensteamworks/IClientSharedConnection.h>

#define PROTOBUF_MASK 0x80000000

// Debugging tips:
// Beautify library.js in {steam install folder}/steamui/library.js
// And launch steam with
// steam -opendevtools -vgui -noverifyfiles -nobootstrapupdate -skipinitialbootstrap -norepairfiles -overridepackageurl
// To use chrome dev tools and avoid overriding the changes to library.js
// Now you can freely modify steam's files to your content, like adding debug logging

// A protobuf message to be sent to the Steam CM's via SharedConnection
template<typename T>
class ProtoMsg 
{
private:
    bool bUnauthenticatedMessage;
    unsigned int eMsg;
    QByteArray bufferByteArray;
    QBuffer *buffer;
    std::string jobname;
public:
    CMsgProtoBufHeader header;
    T body;

    // Used for responses, did this message succeed or did it fail?
    bool success;

    // Sets the EMsg for this message
    void SetEMsg(unsigned int eMsg) {
        this->eMsg = eMsg;
    }

    // Gets the EMsg of this message
    unsigned int GetEMsg() {
        return eMsg;
    }

    // Sets the actual protobuf to be sent
    void SetBody(T body) {
        this->body = body;
    }
    
    // Sets the job_name header of the field
    // It can be a value like Authentication.BeginAuthSessionViaCredentials#1
    void SetJobName(std::string jobName) {
        this->jobname = jobName;
    }

    // Prints the serialized message as Hex to the console
    // You should of course serialize the message first
    void TellHex() {
        #ifndef DEV_BUILD
            #ifdef PRINT_PROTOMSG
                std::cout << "[ProtoMsg] buffer length: " << bufferByteArray.length() << std::endl;
                std::cout << "[ProtoMsg] buffer data: (on next line)" << std::endl;

                // save formatting
                std::ios init(NULL);
                init.copyfmt(std::cout);

                // set formatting 
                std::cout << std::hex << std::setfill('0');  // needs to be set only once
                auto *ptr = reinterpret_cast<unsigned const char *>(bufferByteArray.constData());
                for (int i = 0; i <= bufferByteArray.length(); i++, ptr++) {
                    if (i % sizeof(uint64_t) == 0) {
                        std::cout << std::endl;
                    }
                    std::cout << std::setw(2) << static_cast<unsigned>(*ptr) << " ";
                }

                // restore original formatting
                std::cout.copyfmt(init);
            #endif
        #endif
    }

    // Reads a binary message and copies it to a ProtoMsg object.
    static ProtoMsg<T> FromBinary(const char* binary, size_t length) {
        bool succeeded_parsing_header = true;
        bool succeeded_parsing_body = true;

        // The value of bUnauthenticatedMessage doesn't really matter here
        ProtoMsg<T> msg = new ProtoMsg<T>(false);
        size_t cursorPos = 0;
#ifdef PRINT_PROTOMSG
        DEBUG_MSG << "[ProtoMsg] msg_length " << length << std::endl;
#endif
        // The first part of a message is it's eMsg. This is defined as a LittleEndian uint32
        uint32_t eMsg = BufferHelpers::ReadUint32(binary, length, &cursorPos);
        if (eMsg == -1) {
            std::cerr << "[ProtoMsg] Read a negative uint!" << std::endl;
            msg.success = false;
            return msg;
        }

        // This is how we can "deserialize" the real emsg from the "full" one
        uint32_t eMsgReal = ~PROTOBUF_MASK & eMsg;
        msg.SetEMsg(eMsgReal);

#ifdef PRINT_PROTOMSG
        DEBUG_MSG << "[ProtoMsg] eMsg is " << eMsgReal << std::endl;
#endif

        // The size of the header is defined in the message. 
        // The size of the body isn't defined, as we can just read the rest of the message.
        uint32_t header_size = BufferHelpers::ReadUint32(binary, length, &cursorPos);
        if (header_size == -1) {
            std::cerr << "[ProtoMsg] Read a negative uint!" << std::endl;
            msg.success = false;
            return msg;
        }

        // Read header_size length portion of the message
        // This gets us the header and leaves the cursor where the body begins.
#ifdef PRINT_PROTOMSG
        DEBUG_MSG << "[ProtoMsg] header_size " << header_size << std::endl;
#endif
        char *serializedHeader = BufferHelpers::ReadLength(binary, length, &cursorPos, header_size);
        if (serializedHeader == nullptr) {
            succeeded_parsing_header = false;
        }
            

        // Calculate the remaining bytes and read the body out of them. This is the last part of a message.
        size_t body_size = length - cursorPos;
#ifdef PRINT_PROTOMSG
        DEBUG_MSG << "[ProtoMsg] body_size " << body_size << std::endl;
#endif
        char *serializedBody = BufferHelpers::ReadLength(binary, length, &cursorPos, body_size);
        if (serializedBody == nullptr) {
            succeeded_parsing_body = false;
        }

        // Now comes the interesting parts!

        // First, validate that the eMsg isn't ridiculously large
        // (if there are this many eMsg:s in the future, just bump this number up)
        // This isn't really fatal, but means that something is off. 
        if (eMsgReal > 1147483647) {
            std::cerr << "[ProtoMsg] WARNING: eMsg " << eMsgReal << " is ridiculously large (most likely invalid)" << std::endl;
        }

        try {
            DEBUG_MSG << "[ProtoMsg] Parsing header" << std::endl;

            // Second, check with protobuf that the header is valid by parsing it
            if (!msg.header.ParseFromString(std::string(serializedHeader, header_size))) {
                DEBUG_MSG << "[ProtoMsg] Failed to parse header: " << msg.header.InitializationErrorString() << std::endl;
                succeeded_parsing_header = false;
            }
        } 
        catch (const google::protobuf::FatalException& e) 
        {
            std::cerr << e.what() << std::endl;
            succeeded_parsing_header = false;
        }

        try
        {
            DEBUG_MSG << "[ProtoMsg] Parsing body" << std::endl;

            // Third, check with protobuf that the body is valid by parsing it
            if (!msg.body.ParseFromString(std::string(serializedBody, body_size))) {
                DEBUG_MSG << "[ProtoMsg] Failed to parse body: " << msg.body.InitializationErrorString() << std::endl;
                succeeded_parsing_body = false;
            }
        }
        catch(const google::protobuf::FatalException& e)
        {
            std::cerr << e.what() << std::endl;
            succeeded_parsing_body = false;
        }


        if (!succeeded_parsing_header) {
            std::cerr << "[ProtoMsg] Header failed to parse. Cannot continue." << std::endl;
            return NULL;
        }
        if (!succeeded_parsing_body) {
            std::cerr << "[ProtoMsg] Body failed to parse but header parsed successfully. Is the proper protobuf message specified?" << std::endl;
            std::cerr << "[ProtoMsg] This may also be indicative of user error, like leaving out some required fields." << std::endl;
            msg.success = false;
        } else {
            msg.success = true;
        }
        
#ifdef PRINT_PROTOMSG
        if (succeeded_parsing_header) {
            DEBUG_MSG << "[ProtoMsg] Header: " << std::endl;
            DEBUG_MSG << msg.header.DebugString() << std::endl;
        }

        if (succeeded_parsing_body) {
            DEBUG_MSG << "[ProtoMsg] Body: " << std::endl;
            DEBUG_MSG << msg.body.DebugString() << std::endl;
        }
#endif

        // If we get here, everything has parsed successfully and the ProtoMsg is now ready!
        return msg;
    }

    // Serializes this message to the internal buffer.
    void Serialize()
    {
        if (!this->jobname.empty()) {
            header.set_target_job_name(this->jobname);
        }

        size_t header_size = header.ByteSizeLong();
        size_t body_size = body.ByteSizeLong();

        DEBUG_MSG << "[ProtoMsg] serialize:header_size " << header_size << std::endl;
        DEBUG_MSG << "[ProtoMsg] serialize:body_size " << body_size << std::endl;

        bufferByteArray = QByteArray();
        buffer = new QBuffer(&bufferByteArray);
        buffer->open(QIODevice::Append);
        QDataStream bufferDataStream(buffer);

        // The steamclient.so uses little endian (due to historical reasons or maybe the real client being 32-bit)
        bufferDataStream.setByteOrder(QDataStream::ByteOrder::LittleEndian);

        //bufferDataStream << uint32(PROTOBUF_MASK | eMsg);
        bufferDataStream << uint32_t(PROTOBUF_MASK | eMsg);

        //bufferDataStream << uint32(header_size);
        bufferDataStream << uint32_t(header_size);

        // Serialize the header
        void *header_buf = malloc(header_size);
        header.SerializeToArray(header_buf, header_size);

        // Serialize the body
        void *body_buf = malloc(body_size);
        body.SerializeToArray(body_buf, body_size);

        // write the header
        bufferDataStream.writeRawData((char*)header_buf, header_size);


        // write the body
        bufferDataStream.writeRawData((char*)body_buf, body_size);

        buffer->close();

        return;
    }

    // Call only after you have serialized
    size_t SerializedLength() {
        // Length with null terminator
        return bufferByteArray.length();
    }

#if PROTOMSG_REPARSE
    void ReparseMsg() {
        // Parse our own message and see if it's still valid
        DEBUG_MSG << "[ProtoMsg] Deserializing our own packet; PROTOMSG_REPARSE is set" << std::endl;
        ProtoMsg<T> protoMsg_req_reparsed = ProtoMsg<T>::FromBinary(bufferByteArray.constData(), this->SerializedLength());
        DEBUG_MSG << "[ProtoMsg] reParsed eMsg: " << protoMsg_req_reparsed.GetEMsg() << std::endl;
    }
#endif

    // Sends a message without waiting for a response
    int SendMessage() {
        HSharedConnection conn = Global_SteamClientMgr->ClientSharedConnection->AllocateSharedConnection();
        this->Serialize();
        // Don't need to register an eMsg handler as the value is ignored

#if PROTOMSG_REPARSE
        ReparseMsg();
#endif

        return Global_SteamClientMgr->ClientSharedConnection->SendMessage(conn, bufferByteArray.data(), this->SerializedLength());
    }

    // Sends this message (if valid) and blocks until a response is received.
    // If the response fails to parse, NULL is returned.
    // Before calling, make sure that a valid JobName is set (using SetJobName)
    template<typename ResponseType>
    ProtoMsg<ResponseType> SendMessageAndAwaitResponse() {
        //TODO: is there a better way to do this?
        ResponseType responseReflect;

        this->Serialize();
        this->TellHex();

        HSharedConnection conn = Global_SteamClientMgr->ClientSharedConnection->AllocateSharedConnection();
        Global_SteamClientMgr->ClientSharedConnection->RegisterEMsgHandler(conn, 147);
        Global_SteamClientMgr->ClientSharedConnection->RegisterServiceMethodHandler(conn, jobname.c_str());
        
        #if PROTOMSG_REPARSE
            ReparseMsg();
        #endif

        while (!Global_SteamClientMgr->ClientUser->BConnected())  {
            std::this_thread::sleep_for(std::chrono::milliseconds(50));
        }

        Global_SteamClientMgr->ClientSharedConnection->SendMessageAndAwaitResponse(conn, bufferByteArray.data(), this->SerializedLength());

        CUtlBuffer *responseBuf = new CUtlBuffer(1, 8000000, 0);

        uint32_t rcvdCall;
        
        while (!Global_SteamClientMgr->ClientSharedConnection->BPopReceivedMessage(conn, responseBuf, &rcvdCall))
        {    
            std::this_thread::sleep_for(std::chrono::milliseconds(50));
        }
        DEBUG_MSG << "[ProtoMsg] Received response, try parsing it " << rcvdCall << std::endl;

        DEBUG_MSG << "[ProtoMsg] Deserializing response as " << responseReflect.GetTypeName() << " response length " << responseBuf->TellPut() << std::endl;

        //TODO: Is conversion from unsigned char* to char* safe?
        ProtoMsg<ResponseType> response = ProtoMsg<ResponseType>::FromBinary((char *)responseBuf->m_Memory.Base(), responseBuf->TellPut());

        // Free the buffer afterward
        delete responseBuf;

        // Free the sharedconnection
        Global_SteamClientMgr->ClientSharedConnection->ReleaseSharedConnection(conn);
        return response;
    }

    ProtoMsg(bool bUnauthenticatedMessage) {
        this->bUnauthenticatedMessage = bUnauthenticatedMessage;
        if (this->bUnauthenticatedMessage) {
            this->SetEMsg(k_EMsgServiceMethodCallFromClientNonAuthed);
            header.set_steamid(0);
            
            // This can be generated apparently, look at "steam" python package by ValvePython
            //header.set_client_sessionid(0);
        } else {
            //TODO: set correct sessionid
#ifdef PRINT_PROTOMSG
            DEBUG_MSG << "[ProtoMsg] Current steamid is " << Application::GetApplication()->currentUserSteamID << std::endl;
#endif
            header.set_steamid(Application::GetApplication()->currentUserSteamID);
        }

        // These aren't needed?
        //this->header.set_jobid_source(0);
        //this->header.set_jobid_target(0);
        //this->header.set_messageid(0);
        //this->header.set_timeout_ms(10000);
        //this->header.set_is_from_external_source(false);
        //this->header.set_realm(1);
    }
    ~ProtoMsg() {

    }
};