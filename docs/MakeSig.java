// Finds unique signature for a function (Based of Asher Bakers makesig.idc for IDA pro)


//@author Rachnus, Rosentti
//@keybinding ctrl insert

import ghidra.app.script.GhidraScript;
import ghidra.program.model.listing.*;
import ghidra.program.model.lang.*;
import ghidra.program.model.address.*;

import ghidra.util.HashUtilities;

public class MakeSig extends GhidraScript 
{
    public void run() throws Exception 
	{
		Function func = getCursorFunction();
		if(func == null)
		{
			println("Error: Select a function first.");
			return;
		}
		
		Address startAddr = getFunctionStartAddr(func);
		Address endAddr = getFunctionEndAddr(func);

		printf("Finding unique sig for function: %s (%s)", func.getName(), func.getSignature().getPrototypeString());
		boolean found = false;
		String sig = "";

		// Add .next to endAddr to include return statement
		for(Address currentAddr = startAddr; !currentAddr.equals(endAddr.next()) && !monitor.isCancelled(); currentAddr = currentAddr.next())
		{
			// Get instruction at current address
			Instruction ins = getInstructionAt(currentAddr);
			
			///println("CURRENT ADDR: " + currentAddr + " - " + ins);
			if(ins != null)
			{
				// Opcodes for this instruction
				byte[] bytes = ins.getBytes();

				for(int i = 0; i < bytes.length; i++)
				{
					///printf("OP TYPE: %d (0x%X)\n", ins.getOperandType(i), bytes[i]);
					
					int opType = ins.getOperandType(i);
					if((opType & OperandType.DYNAMIC) == OperandType.DYNAMIC)
					{
						// Wildcard dynamic operands
						sig += ".";
					}
					else
					{
						// Else append the byte code
						sig += String.format("\\x%02X", bytes[i]);
					}
					
					if(isSigUnique(sig))
					{
						found = true;
						break;
					}		
				}
				
				if(found)
					break;
			}
		}
		
		if(!found)
		{
			printf("Could not find unique signature.");
			return;
		}
		
		// Do not print of we cancelled script
		if(monitor.isCancelled())
			return;
		
		printf("--- %s SIGS ---\n", func.getName());
		printf("GHIDRA: %s\n", sig);
		printf("IDA: %s\n", ghidraSigToIDASig(sig));
		printf("SM: %s\n", ghidraSigToSourceModSig(sig));
		printf("OpenSteam: FindSignature(\"%s\", \"%s\")\n", ghidraSigToOpenSteamSig(sig), ghidraSigToOpenSteamSigMask(sig));
	}
	
	/**
	 * Gets the start address of a function
	 *
	 * @param Function    function object
	 * @return Address    function start address
	 */
	public Address getFunctionStartAddr(Function func)
	{
		AddressSetView setView = func.getBody();
		return setView.getMinAddress();
	}
	
	/**
	 * Gets the end address of a function
	 *
	 * @param Function    function object
	 * @return Address    function end address
	 */
	public Address getFunctionEndAddr(Function func)
	{
		AddressSetView setView = func.getBody();
		return setView.getMaxAddress();
	}
	
	/**
	 * Returns array of addresses found by signature
	 *
	 * @param sig         signature string
	 * @return Address[]  array of addresses
	 */
	public Address[] findSigAddr(String sig, int count)
	{
		return findBytes(null, sig, count);
	}
	
	/**
	 * Checks if signature is unique
	 *
	 * Dot (.) is used as wildcard
	 * Example sig string using wildcards: \x55\x8B.\x55....
	 * is equivalent to 
	 *                   55 8B ? 55 ? ? ? ?                    // IDA
	 *                   \x55\x8B\x2A\x55\x2A\x2A\x2A\x2A      // SOURCEMOD
	 *
	 * @param sig       signature string
	 * @return bool     true if unique
	 */
	public boolean isSigUnique(String sig)
	{
		// If it finds more than 1 sig, its not unique
		return findSigAddr(sig, 2).length == 1;
	}
	
	/**
	 * Converts ghidra sig to SourceMod sig
	 *
	 * @param sig       ghidra sig
	 * @return String   SM sig
	 */
	public String ghidraSigToSourceModSig(String sig)
	{
		return sig.replaceAll("\\.", "\\\\x2A");
	}

	/**
	 * Converts ghidra sig to OpenSteam sig
	 *
	 * @param sig       ghidra sig
	 * @return String   SM sig
	 */
	public String ghidraSigToOpenSteamSig(String sig)
	{
		return sig.replaceAll("\\.", "\\\\x00");
	}

	/**
	 * Converts ghidra sig to OpenSteam sigmask
	 *
	 * @param sig       ghidra sig
	 * @return String   SM sig
	 */
	public String ghidraSigToOpenSteamSigMask(String sig)
	{
		StringBuilder sb = new StringBuilder(sig);
		StringBuilder mask = new StringBuilder();
		for(int i = 0; i < sb.length(); i++)
		{
			if(sb.charAt(i) == '\\')
			{
				mask.append("x");
			} else if (sb.charAt(i) == '.') {
				mask.append("?");
			}
		}
		
		return mask.toString();
	}
	
	/**
	 * Converts ghidra sig to IDA sig
	 *
	 * @param sig       ghidra sig
	 * @return String   IDA sig
	 */
	public String ghidraSigToIDASig(String sig)
	{
		StringBuilder sb = new StringBuilder(sig);
		for(int i = 0; i < sb.length(); i++)
		{
			if(sb.charAt(i) == '\\')
				sb.deleteCharAt(i);
		}
		
		return sb.toString().replaceAll("\\.", " ? ").replaceAll("x", " ");
	}
	
	/**
	 * Gets selected function
	 *
	 * @return void
	 */
	public Function getCursorFunction()
	{
		AddressSetView scope = currentSelection;
		if (scope == null) 
			return getFunctionContaining(currentAddress);
		
		return null;
	}
}
