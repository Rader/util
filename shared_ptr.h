//
//  shared_ptr.h
//  learncpp
//
//  Created by Rader on 12/5/16.
//  Copyright © 2016 Rader. All rights reserved.
//

#ifndef shared_ptr_h
#define shared_ptr_h

#include <iostream>

namespace rader
{

class counter
{
  public:
    int count;

    counter(int i) : count(1) {}
};

template <typename T>
class shared_ptr
{

  public:
    shared_ptr(T *p) : shared_counter(new counter(1))
    {
        raw_p = p;
        std::cout << "shared_ptr created:" << this->shared_counter->count << endl;
    }

    // Increase the reference count when shared_ptr is copied.
    shared_ptr(shared_ptr &shared_p)
    {
        this->raw_p = shared_p.raw_p;
        this->shared_counter = shared_p.shared_counter;
        this->shared_counter->count++;
        std::cout << "reference increased to:" << this->shared_counter->count << endl;
    }

    T *operator->()
    {
        return raw_p;
    }

    T &operator&()
    {
        return *raw_p;
    }

    ~shared_ptr()
    {
        this->shared_counter->count--;
        std::cout << "count decreased to:" << this->shared_counter->count << endl;
        // destroy the raw pointer when there is no reference
        if (this->shared_counter->count == 0)
        {
            delete raw_p;
            cout << "delete raw_p" << endl;

            delete shared_counter;
            cout << "delete shared_counter" << endl;
        }
    }

    T *raw_p;
    // Wrap the count, so it can be shared by reference between different shared_ptr instances.
    counter *shared_counter;
};
}
#endif /* shared_ptr_h */
