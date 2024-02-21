/*************************/
/* Auto generated script */
/*************************/

import { ContractStatesEnum } from '../enums/ContractEnums.enum';

export interface IGameContractDto {
    
    value: number | null;
    color: number | null;
    lastPlayerNumber: number;
    currentPlayerNumber: number;
    turnEndTime: Date;
    lastColor: number | null;
    lastValue: number | null;
    isContractCoinched: boolean;
    isContractCounterCoinched: boolean;
    owningTeam: number | null;
    contractState: ContractStatesEnum;
}

