import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../services/account.service';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  registerForm: FormGroup;
  validationErrors: string[] = [];

  /**
   *
   */
  constructor(private accountService: AccountService, private router: Router) {

    
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      knownAs: new FormControl('', Validators.required),
      gender: new FormControl('male'),
      dateOfBirth: new FormControl(new Date().toISOString().substring(0, 10), Validators.required),
      city: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')])
    })
    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['confirmPassword'].updateValueAndValidity(); 
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.get(matchTo)?.value ? null : { isMatching: true };
    }
  }

  register(): Subscription {
    return this.accountService.register(this.registerForm.value).subscribe({
      next: response => {
        this.router.navigateByUrl('/members');
      },
      error: error => {
        this.validationErrors = error;
      }
    });
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
