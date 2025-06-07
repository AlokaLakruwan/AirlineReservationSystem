import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService, User } from '../../../services/user.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule, 
    HttpClientModule, 
    ReactiveFormsModule
  ],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  loading = false;
  success = '';
  error = '';
  showForm = false;
  isEditMode = false;
  currentUserId: number | null = null;
  userForm!: FormGroup;

  constructor(
    private userService: UserService, 
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.fetchUsers();
    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      roleId: [1, [Validators.required, Validators.min(1)]],  // default role = 1
      isActive: [true],
    });
  }

  fetchUsers(): void {
    this.loading = true;
    this.userService.getAll().subscribe({
      next: (data) => {
        this.users = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load users.';
        console.error(err);
        this.loading = false;
      }
    });
  }

  openCreateForm(): void {
    this.isEditMode = false;
    this.currentUserId = null;
    this.userForm.addControl(
      'password',
      this.fb.control('', [Validators.required, Validators.minLength(5)])
    );
    this.userForm.reset({
      username: '',
      firstName: '',
      lastName: '',
      email: '',
      roleId: 1,
      isActive: true,
      password: '',
    });
    this.showForm = true;
  }

  openEditForm(user: User): void {
    this.isEditMode = true;
    this.currentUserId = user.userId;
    if (this.userForm.contains('password')) {
      this.userForm.removeControl('password');
    }
    this.userForm.setValue({
      username: user.username,
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      roleId: user.roleId,
      isActive: user.isActive,
    });
    this.showForm = true;
  }

  saveUser(): void {
    this.error = '';
    this.success = '';

    if (this.userForm.invalid) {
      this.error = 'Please fill in all required fields correctly.';
      return; 
    }

    const formValue = this.userForm.value as {
      username:  string;
      firstName: string;
      lastName:  string;
      email:     string;
      roleId:    number;
      isActive:  boolean;
      password?:  string;
    };

    const payload: any = {
      username:  formValue.username,
      firstName: formValue.firstName,
      lastName:  formValue.lastName,
      email:     formValue.email,
      roleId:    formValue.roleId,
      isActive:  formValue.isActive,
      createdAt: new Date().toISOString()   
    };

    if (!this.isEditMode) {
      payload.password = formValue.password!;
    }

    if (this.isEditMode && this.currentUserId != null) {
      payload.userId = this.currentUserId;
      this.userService.update(this.currentUserId, payload).subscribe({
        next: () => {
          this.success = 'User updated successfully!';
          this.fetchUsers();  
          this.showForm = false;
          this.autoClearMessages();
        },
        error: (err: any) => {
          console.error('Error updating user:', err);
          this.error = 'Error updating user.';
          this.autoClearMessages();
        }
      });
    } else {
      // Create new user
      this.userService.create(payload).subscribe({
        next: () => {
          this.success = 'User created successfully!';
          this.fetchUsers();
          this.showForm = false;
          this.autoClearMessages();
        },
        error: (err: any) => {
          this.error = 'Error creating user.';
          console.error('Error creating user:', err);
          this.autoClearMessages();
        }
      });
    }
  }

  confirmDelete(id: number): void {
    this.error = '';
    this.success = '';

    if (confirm('Delete this user?')) {
      this.userService.delete(id).subscribe({
        next: () => {
          this.success = 'User deleted successfully!';
          this.fetchUsers();
          this.autoClearMessages();
        },
        error: (err: any) => {
          this.error = 'Error deleting user.';
          console.error('Error deleting user:', err);
          this.autoClearMessages();
        }
      });
    }
  }

  cancelForm(): void {
    this.showForm = false;
  }

  private autoClearMessages(): void {
    setTimeout(() => {
      this.error = '';
      this.success = '';
    }, 3000);
  }
}