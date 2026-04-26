import { BrandOwnershipStatus } from "./enums/brand-ownership-enum";
import { TrademarkStatus } from "./enums/trademark-status-enum";

export interface SellerStoreInformation {

storeName: string;
hasUPCsForAllProcucts: boolean;
hasDiversityCertification: boolean;
brandOwnership: BrandOwnershipStatus;
trademarkStatus: TrademarkStatus;

}