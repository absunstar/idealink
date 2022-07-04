import { UserPermission } from "./user-permission";

export class UserProfile {
    id: string;
    FullName: string;
    Role: string;
    // lastName: string;
     userPermissions: UserPermission[];
}