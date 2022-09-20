export interface Menu {
  name: string;
  validUrl: string;
  id: number;
  parentId: number;
  displayOrder: number;
  subMenu: Menu[];
}
