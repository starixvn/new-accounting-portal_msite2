import { of } from 'rxjs';
import { Menu } from './nav-menu.model';
import { NavMenuService } from './nav-menu.service.native';

describe('NavMenuComponent', () => {
  let component: NavMenuComponent;
  let fixture: ComponentFixture<NavMenuComponent>;
  let mockMenuService: any;

  const mockMenus: Menu[] = [
    {
      id: 1,
      name: 'Main',
      displayOrder: 1,
      url: '/main',
      parentId: 0,
      validUrl: '/main',
      subMenu: [],
    },
    {
      id: 2,
      name: 'Sub',
      displayOrder: 2,
      url: '/sub',
      parentId: 1,
      validUrl: '/sub',
      subMenu: [],
    },
  ];

  beforeEach(() => {
    mockMenuService = {
      getMenu: jasmine
        .createSpy('getMenu')
        .and.returnValue(of({ menus: mockMenus, userName: 'testUser' })),
    };

    TestBed.configureTestingModule({
      declarations: [NavMenuComponent],
      providers: [{ provide: NavMenuService, useValue: mockMenuService }],
    });
    fixture = TestBed.createComponent(NavMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call getMenu and set menus and userName on loadMenu', () => {
    component.loadMenu();
    expect(mockMenuService.getMenu).toHaveBeenCalled();
    expect(component.menus).toBeTruthy();
    expect(component.userName).toBe('testUser');
  });

  it('should format url correctly in getUrl', () => {
    const url = '/root/child/page';
    expect(component.getUrl(url)).toBe('child/page');
  });

  it('should build nested subMenu structure', () => {
    component.loadMenu();
    expect(component.menus![0].subMenu.length).toBe(1);
    expect(component.menus![0].subMenu[0].id).toBe(2);
  });

  it('should add Provider menu with correct structure', () => {
    component.loadMenu();
    const providerMenu = component.menus!.find((m) => m.id === 1048);
    expect(providerMenu).toBeTruthy();
    expect(providerMenu!.subMenu.length).toBe(1);
    expect(providerMenu!.subMenu[0].name).toBe('Mapping Provider to Sport');
  });
});
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavMenuComponent } from './nav-menu.component';

describe('NavMenuComponent', () => {
  let component: NavMenuComponent;
  let fixture: ComponentFixture<NavMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NavMenuComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NavMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
